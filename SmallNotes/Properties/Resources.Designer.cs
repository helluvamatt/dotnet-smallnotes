﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmallNotes.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SmallNotes.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Another instance is already running! Exiting....
        /// </summary>
        internal static string AnotherInstance {
            get {
                return ResourceManager.GetString("AnotherInstance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SmallNotes.
        /// </summary>
        internal static string AppTitle {
            get {
                return ResourceManager.GetString("AppTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to E&amp;xit.
        /// </summary>
        internal static string MenuItemExit {
            get {
                return ResourceManager.GetString("MenuItemExit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;Manage.
        /// </summary>
        internal static string MenuItemManage {
            get {
                return ResourceManager.GetString("MenuItemManage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;New Note.
        /// </summary>
        internal static string MenuItemNewNote {
            get {
                return ResourceManager.GetString("MenuItemNewNote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Program Terminated Unexpectedly.
        /// </summary>
        internal static string ProgramTerminated {
            get {
                return ResourceManager.GetString("ProgramTerminated", resourceCulture);
            }
        }
    }
}
