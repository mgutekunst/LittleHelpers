using System;
using System.Collections.Generic;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.Reflection;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Feature.Services.Html;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Feature.Services.Xaml.Bulbs;
using JetBrains.ReSharper.Feature.Services.Xml.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Intentions.Xml.ContextActions;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xaml.Impl;
using JetBrains.ReSharper.Psi.Xaml.Impl.Util;
using JetBrains.ReSharper.Psi.Xaml.Tree;
using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
using JetBrains.ReSharper.Psi.Xml.Parsing;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace LittleHelpers.ContextActions
{
    [ContextAction(Description = "Changes Visibility in Xaml from Visible to collapsed and the other way round",
      Group = "XAML",
      Name = "XAMLVisibilitySwitcher")]
    public sealed class XAMLVisibilitySwitcher : IContextAction
    {   
        private readonly XmlContextActionDataProvider _provider;
        private IBulbAction[] _items;

        /// <summary>
        /// For languages other than C# any inheritor of <see cref="IContextActionDataProvider"/> can 
        /// be injected in this constructor.
        /// </summary>
        public XAMLVisibilitySwitcher(XmlContextActionDataProvider provider)
        {
            _provider = provider;
        }


        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return Items.ToContextAction();
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            var field = _provider.GetSelectedElement<IPropertyAttribute>(true, true);
            if(field != null)
            {
                if(field.PropertyName == "Visibility")
                {
                    var text = field.Value.GetText();
                    return text.Contains("Visible") || text.Contains("Collapsed");
                }
//                var vis = field.Attributes.FirstOrDefault(a => a.AttributeName == "Visibility");
//                return vis != null;
            }

            return false;
        }

        private IBulbAction[] Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new IBulbAction[]
                   {
                     new XamlBulbItem(_provider)
                   };
                }
                return _items;
            }
        }
    }

    public class XamlBulbItem : BulbActionBase
    {
        private readonly XmlContextActionDataProvider _provider;

        public XamlBulbItem(XmlContextActionDataProvider provider)
        {
            _provider = provider;
        }

        public override string Text
        {
            get
            {
                // text returned here will be displayed on the context action
                return "Switch Visibility";
            }
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var field = _provider.GetSelectedElement<IPropertyAttribute>(true, true);
            if (field != null)
            {
                var factory = XamlElementFactory.GetInstance(_provider.PsiModule);
                IXmlAttribute vis;
                if(field.Value.GetText().Contains("Visible"))
                {
                    field.SetStringValue("Collapsed");
                    
                } else if (field.GetText().Contains("Collapsed"))
                {
                    field.SetStringValue("Visible");
                }
            }

            return null;
        }

    }
}
