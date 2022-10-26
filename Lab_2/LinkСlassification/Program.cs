using HtmlAgilityPack;
using System.Net;

// http://links.qatl.ru/
// https://www.unisender.com/
string domen = @"https://www.unisender.com/";

void LinkStatus(string url, StreamWriter validLinks, StreamWriter invalidLinks, ref int countValidLinks, ref int countInvalidLinks)
{
    try
    {
        HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
        request.Method = "HEAD";
        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        validLinks.Write($"{url} ");
        validLinks.WriteLine((int)response.StatusCode);
        countValidLinks++;
    }
    catch (WebException ex)
    {
        int code = (int)((HttpWebResponse)ex.Response).StatusCode;
        if (code < 400)
        {
            validLinks.Write($"{url} ");
            validLinks.WriteLine(code);
            countValidLinks++;
        }
        else
        {
            invalidLinks.Write($"{url} ");
            invalidLinks.WriteLine(code);
            countInvalidLinks++;
        }
    }
}

void CheckAllLinks(string href, List<string> uniqueHref)
{
    string url = "";
    url = domen + href;
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
        if (href.IndexOf(domen) == 0)
        {
            href = href.Substring(domen.Length);
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
            if (!uniqueHref.Contains(href))
            {
                uniqueHref.Add(href);
                CheckAllLinks(href, uniqueHref);
            }
        }
    }
}



string emptyLink = "";
List<string> uniqueHref = new List<string>();
using StreamWriter validLinks = new("../../../validLinks.txt");
using StreamWriter invalidLinks = new("../../../invalidLinks.txt");

int countValidLinks = 0, countInvalidLinks = 0;
LinkStatus(domen, validLinks, invalidLinks, ref countValidLinks, ref countInvalidLinks);
CheckAllLinks(emptyLink, uniqueHref);
string url = "";

foreach (string link in uniqueHref)
{
    url = domen + link;   
    LinkStatus(url, validLinks, invalidLinks, ref countValidLinks, ref countInvalidLinks);
}

validLinks.WriteLine($"Quantity: {countValidLinks}");
validLinks.WriteLine(DateTime.Now);
invalidLinks.WriteLine($"Quantity: {countInvalidLinks}");
invalidLinks.WriteLine(DateTime.Now);