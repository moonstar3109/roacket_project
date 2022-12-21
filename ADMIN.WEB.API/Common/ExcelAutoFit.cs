using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace ADMIN.WEB.API.Common
{
    public class ExcelAutoFit
    {
		public void ExAutoFitColumns(CellRange cellRange)
		{
			for (int row = cellRange.Row; row <= cellRange.LastRow; row++)
			{
				for (int col = cellRange.Column; col <= cellRange.LastColumn; col++)
				{
					CellRange oneCellRange = cellRange[row, col];
					double oldWidth = oneCellRange.ColumnWidth;
					double newWidth = 0;
					string text = oneCellRange.DisplayedText;
					if (string.IsNullOrWhiteSpace(text) == false)
					{
						using (Font nativeFont = oneCellRange.Style.Font.GenerateNativeFont())
						{
							newWidth = (double)TextRenderer.MeasureText(text, nativeFont).Width;
							newWidth = (newWidth - (double)12) / 7d + 1;    //From Picxels to inches
						}
					}

					if (newWidth > oldWidth)
					{
						oneCellRange.ColumnWidth = newWidth;
						oldWidth = newWidth;
					}
				}
			}
		}
    }
}