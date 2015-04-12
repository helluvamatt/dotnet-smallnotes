using Common.Data;
using Common.UI.LocalizedDesignAttributes;
using SmallNotes.Properties;
using SmallNotes.UI.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallNotes.Data.Entities
{
	public class Settings
	{
		public const string DEFAULT_BACKGROUND_COLOR = "#FBEC94";

		[LocalizedDisplayName(typeof(Resources), "Settings_FastRendering_DisplayName"), LocalizedDescription(typeof(Resources), "Settings_FastRendering_Description"), LocalizedCategory(typeof(Resources), "Settings_Category_Advanced"), DefaultValue(true)]
		public bool FastRendering { get; set; }

		[LocalizedDisplayName(typeof(Resources), "Settings_CustomCss_DisplayName"), LocalizedDescription(typeof(Resources), "Settings_CustomCss_Description"), LocalizedCategory(typeof(Resources), "Settings_Category_Advanced"), DefaultValue(""), Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		public string CustomCss { get; set; }

		[
		LocalizedDisplayName(typeof(Resources), "Settings_DefaultBackgroundColor_DisplayName"),
		LocalizedDescription(typeof(Resources), "Settings_DefaultBackgroundColor_Description"),
		LocalizedCategory(typeof(Resources), "Settings_Category_Defaults"),
		DefaultValue(DEFAULT_BACKGROUND_COLOR),
		Editor(typeof(BackgroundColorEditor), typeof(UITypeEditor))
		]
		public string DefaultBackgroundColor { get; set; }

		[
		LocalizedDisplayName(typeof(Resources), "Settings_DefaultForegroundColor_DisplayName"),
		LocalizedDescription(typeof(Resources), "Settings_DefaultForegroundColor_Description"),
		LocalizedCategory(typeof(Resources), "Settings_Category_Defaults"),
		DefaultValue(null),
		Editor(typeof(ForegroundColorEditor), typeof(UITypeEditor)),
		TypeConverter(typeof(ForegroundColorEditor.ForegroundColorTypeConverter))
		]
		public string DefaultForegroundColor { get; set; }

		[
		LocalizedDisplayName(typeof(Resources), "Settings_DefaultFont_DisplayName"),
		LocalizedDescription(typeof(Resources), "Settings_DefaultFont_Description"),
		LocalizedCategory(typeof(Resources), "Settings_Category_Defaults")
		]
		public Font DefaultFont { get; set; }

		[
		LocalizedDisplayName(typeof(Resources), "Settings_DefaultSize_DisplayName"),
		LocalizedDescription(typeof(Resources), "Settings_DefaultSize_Description"),
		LocalizedCategory(typeof(Resources), "Settings_Category_Defaults"),
		DefaultValue(typeof(Size), "300, 300")
		]
		public Size DefaultSize { get; set; }

		[
		LocalizedDisplayName(typeof(Resources), "Settings_IdleTimeout_DisplayName"),
		LocalizedDescription(typeof(Resources), "Settings_IdleTimeout_Description"),
		LocalizedCategory(typeof(Resources), "Settings_Category_Preferences"),
		Editor(typeof(IdleTimeoutEditor), typeof(UITypeEditor)),
		TypeConverter(typeof(IdleTimeoutEditor.IdleTimeoutConverter))
		]
		public long IdleTimeout { get; set; }

		[Browsable(false)]
		public string DatabaseType { get; set; }

		[Browsable(false)]
		public Dictionary<string, string> DatabaseProperties { get; set; }

		[Browsable(false), DefaultValue("LargeIcon")]
		public string NotesListView { get; set; }
	}
}
