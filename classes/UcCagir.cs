using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace MyWpfApp.classes
{
    class UcCagir
    {
        public static void Uc_Cagir(Grid grd ,UserControl uc)
        {
            if (grd.Children.Count > 0)
            {
                grd.Children.Clear();
                grd.Children.Add(uc);

            }
            else
            {
                grd.Children.Add(uc);
            }
        }
    }
    
}
