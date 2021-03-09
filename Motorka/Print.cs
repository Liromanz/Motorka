using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Printing;

namespace Motorka
{
    class Print
    {
        private static PrintManager printMan;
        private static PrintDocument printDoc = new PrintDocument();
        private static IPrintDocumentSource printDocSource;
        private static RichTextBlock textBlock;
        private static bool methods = false;

        public static void SetPrint(RichTextBlock richTextBlock)
        {
            textBlock = richTextBlock;
            printMan = PrintManager.GetForCurrentView();
            printDocSource = printDoc.DocumentSource;
            if (!methods)
            {
                printMan.PrintTaskRequested += PrintTaskRequested;
                printDoc.Paginate += Paginate;
                printDoc.GetPreviewPage += GetPreviewPage;
                methods = true;
            }
        }

        private static void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            var printTask = args.Request.CreatePrintTask("Print", PrintTaskSourceRequrested);
        }
        private static void PrintTaskSourceRequrested(PrintTaskSourceRequestedArgs args)
        {
            args.SetSource(printDocSource);
        }
        private static void Paginate(object sender, PaginateEventArgs e)
        {
            printDoc.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }
        private static void GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            printDoc.SetPreviewPage(e.PageNumber, textBlock);
        }
    }
}
