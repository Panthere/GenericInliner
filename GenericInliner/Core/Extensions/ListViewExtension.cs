using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClientPlugin.Models;

namespace GenericInliner.Core.Extensions
{
    public static class ListViewExtension
    {
        /// <summary>
        /// Add a plugin row to a listview
        /// </summary>
        /// <param name="lv">The listview to add to</param>
        /// <param name="plugin">The plugin to add a row for</param>
        public static void AddPluginRow(this ListView lv, IGenericInliner plugin)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = plugin.Name;
            lvi.SubItems.Add(plugin.Description);
            lvi.SubItems.Add(plugin.Author);
            lvi.SubItems.Add(string.Format("v{0}", plugin.Version));
            lvi.SubItems.Add(plugin.Controls.Count > 0 ? "Yes" : "No");
            lv.Items.Add(lvi);
        }
    }
}
