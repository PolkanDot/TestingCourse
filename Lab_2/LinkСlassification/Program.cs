using System.Net;
using HtmlAgilityPack;

void SortingLinks(StreamWriter validOut, StreamWriter invalidOut, ref List<string> allLinks)
{
    int statusCode, validLinksCount = 0, invalidLinksCount = 0;
    foreach (string link in allLinks)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
            request.Method = WebRequestMethods.Http.Head;
            request.Accept = @"*/*";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            statusCode = (int)response.StatusCode;
            response.Close();
        }
        catch (WebException ex)
        {
            if (ex.Response == null)
                throw;
            statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
        }
        if (statusCode > 399)
        {
            invalidOut.WriteLine(link + " " + statusCode.ToString());
            invalidLinksCount++;
        }
        else
        {
            validOut.WriteLine(link + " " + statusCode.ToString());
            validLinksCount++;
        }
    }
    invalidOut.WriteLine();
    validOut.WriteLine();
    invalidOut.WriteLine(invalidLinksCount.ToString());
    invalidOut.WriteLine(DateTime.Now);
    validOut.WriteLine(validLinksCount.ToString());
    validOut.WriteLine(DateTime.Now);
}

void SearchingLinks(ref HtmlWeb hap, string baseLink, ref List<string> allLinks, ref string baseDomen)
{
    Uri uriAddress;
    var document = hap.Load(baseLink);
    var nodes = document.DocumentNode.SelectNodes("//a");
    string baseLincCopy = baseLink;
    string baseDomenCopy = baseDomen;
    if (nodes != null)
    {
        foreach (HtmlNode node in nodes)
        {
            string currentLink = node.GetAttributeValue("href", null);
            if ((currentLink != null) &&
                (!allLinks.Exists(element => ((element == currentLink) || (element == baseDomenCopy + currentLink)))))
            {
                if (currentLink.IndexOf(baseDomen) == 0)
                {
                    allLinks.Insert(allLinks.IndexOf(baseLink) + 1, currentLink);
                    Console.WriteLine(currentLink);
                }
                else if (!(currentLink.Contains(":")) && (Uri.TryCreate(currentLink, UriKind.Relative, out uriAddress)))
                {
                    allLinks.Insert(allLinks.IndexOf(baseLink) + 1, baseDomen + currentLink);
                    Console.WriteLine(baseDomen + currentLink);
                }
            }
        }
    }
    int nextLinkIndex = allLinks.IndexOf(baseLink) + 1;
    if (allLinks.Count > nextLinkIndex)
    {
        SearchingLinks(ref hap, allLinks[nextLinkIndex], ref allLinks, ref baseDomen);
    }

}
Console.WriteLine("Type path to the file:");
string domen = Console.ReadLine();
HtmlWeb web = new HtmlWeb();
List<string> links = new List<string>();
using StreamWriter swValid = new ("../../../validLinks.txt");
using StreamWriter swInvalid = new ("../../../invalidLinks.txt");

try
{
    SearchingLinks(ref web, domen, ref links, ref domen);
    SortingLinks(swValid, swInvalid, ref links);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

swValid.Close();
swInvalid.Close();