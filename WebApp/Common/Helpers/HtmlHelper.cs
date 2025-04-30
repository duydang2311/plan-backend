using HtmlAgilityPack;

namespace WebApp.Common.Helpers;

public static class HtmlHelper
{
    /// <summary>
    /// Converts HTML to plain text / strips tags.
    /// </summary>
    /// <param name="html">The HTML.</param>
    /// <returns></returns>
    public static string ConvertToPlainText(string html, int maxLength = -1)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var sw = new StringWriter();
        ConvertTo(doc.DocumentNode, sw, maxLength);
        sw.Flush();
        return sw.ToString();
    }

    /// <summary>
    /// Count the words.
    /// The content has to be converted to plain text before (using ConvertToPlainText).
    /// </summary>
    /// <param name="plainText">The plain text.</param>
    /// <returns></returns>
    public static int CountWords(string plainText)
    {
        return !string.IsNullOrEmpty(plainText) ? plainText.Split(' ', '\n').Length : 0;
    }

    public static string Cut(string text, int length)
    {
        if (!string.IsNullOrEmpty(text) && text.Length > length)
        {
            text = string.Concat(text.AsSpan(0, length - 4), " ...");
        }
        return text;
    }

    private static void ConvertContentTo(HtmlNode node, StringWriter sw, int maxLength)
    {
        foreach (var subnode in node.ChildNodes)
        {
            if (maxLength != -1 && sw.GetStringBuilder().Length >= maxLength)
            {
                return;
            }
            ConvertTo(subnode, sw, maxLength);
        }
    }

    private static void ConvertTo(HtmlNode node, StringWriter sw, int maxLength)
    {
        if (maxLength != -1 && sw.GetStringBuilder().Length >= maxLength)
        {
            return;
        }

        string html;
        switch (node.NodeType)
        {
            case HtmlNodeType.Comment:
                // don't output comments
                break;

            case HtmlNodeType.Document:
                ConvertContentTo(node, sw, maxLength);
                break;

            case HtmlNodeType.Text:
                // script and style must not be output
                var parentName = node.ParentNode.Name;
                if ((parentName == "script") || (parentName == "style"))
                {
                    break;
                }

                // get text
                html = ((HtmlTextNode)node).Text;

                // is it in fact a special closing node output as text?
                if (HtmlNode.IsOverlappedClosingElement(html))
                {
                    break;
                }

                // check the text is meaningful and not a bunch of whitespaces
                if (html.Trim().Length > 0)
                {
                    sw.Write(HtmlEntity.DeEntitize(html));
                }
                break;

            case HtmlNodeType.Element:
                switch (node.Name)
                {
                    case "p":
                        // treat paragraphs as crlf
                        sw.Write("\r\n");
                        break;
                    case "br":
                        sw.Write("\r\n");
                        break;
                }

                if (node.HasChildNodes)
                {
                    ConvertContentTo(node, sw, maxLength);
                }
                break;
        }
    }
}
