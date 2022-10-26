using HtmlAgilityPack;
using System.Net;

// http://links.qatl.ru/
// https://www.unisender.com/

void SortingLinks(ref List<string> allUniqueHref, ref string baseDomen, StreamWriter validLinksStream, StreamWriter invalidLinksStream, ref int countValidLinks, ref int countInvalidLinks)
{
    string url;
    foreach (string link in allUniqueHref)
    {
        url = baseDomen + link;
        try
        {
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "HEAD";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            validLinksStream.Write(url);
            validLinksStream.WriteLine((int)response.StatusCode);
            countValidLinks++;
        }
        catch (WebException ex)
        {
            int code = (int)((HttpWebResponse)ex.Response).StatusCode;
            if (code < 400)
            {
                validLinksStream.Write($"{url} ");
                validLinksStream.WriteLine(code);
                countValidLinks++;
            }
            else
            {
                invalidLinksStream.Write($"{url} ");
                invalidLinksStream.WriteLine(code);
                countInvalidLinks++;
            }
        }
    }
    validLinksStream.WriteLine();
    invalidLinksStream.WriteLine();
    validLinksStream.WriteLine($"Quantity: {countValidLinks}");
    validLinksStream.WriteLine(DateTime.Now);
    invalidLinksStream.WriteLine($"Quantity: {countInvalidLinks}");
    invalidLinksStream.WriteLine(DateTime.Now);
}

void SearchingLinks(string href, ref List<string> allUniqueHref, ref string baseDomen)
{
    string url = "";
    url = baseDomen + href;
    HtmlWeb web = new HtmlWeb();
    var htmlDoc = web.Load(url);
    var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");
    if (htmlNodes == null)
    {
        return;
    }
    foreach (HtmlNode node in htmlNodes.ToList())
    {
        href = node.Attributes["href"].Value;
        // если абсолютная ссылка
        if (href.IndexOf(baseDomen) == 0)
        {
            href = href.Substring(baseDomen.Length);
        }
        // если внешняя ссылка
        else if (href.Contains(':'))
        {
            continue;
        }
        // если относительная ссылка
        else if (href.Length > 2)
        {
            if (href[0] == '#')
            {
                continue;
            }
            if (href[0] == '.')
            {

                href = href.Substring(3);
            }
            if (href[0] == '/')
            {

                href = href.Substring(1);
            }
            if (!allUniqueHref.Contains(href))
            {
                allUniqueHref.Add(href);
                SearchingLinks(href, ref allUniqueHref, ref baseDomen);
            }
        }
    }
}


Console.WriteLine("Type path to the file:");
string domen = Console.ReadLine();
if (domen[domen.Length - 1] != '/')
{
    domen = domen + "/";
}

string emptyLink = "";
List<string> allUniqueHref = new List<string>();
using StreamWriter validLinksStream = new("../../../validLinks.txt");
using StreamWriter invalidLinksStream = new("../../../invalidLinks.txt");

int countValidLinks = 0, countInvalidLinks = 0;
SearchingLinks(emptyLink, ref allUniqueHref, ref domen);
SortingLinks(ref allUniqueHref, ref domen, validLinksStream, invalidLinksStream, ref countValidLinks, ref countInvalidLinks);