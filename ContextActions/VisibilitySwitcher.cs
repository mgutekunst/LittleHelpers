using System;
using System.Collections.Generic;
using System.Windows;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace LittleHelpers.ContextActions
{
    [ContextAction(Description = "Switch the Visibilty enum from Visible to collapsed and the other way round",
      Group = "C#",
      Name = "VisibilitySwitcher")]
    public sealed class VisibilitySwitcher : IContextAction
    {
        private readonly ICSharpContextActionDataProvider _provider;
        private IBulbAction[] _items;

        /// <summary>
        /// For languages other than C# any inheritor of <see cref="IContextActionDataProvider"/> can 
        /// be injected in this constructor.
        /// </summary>
        public VisibilitySwitcher(ICSharpContextActionDataProvider provider)
        {
            _provider = provider;
        }


        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return Items.ToContextAction();
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            // Availability code may be optimized but for most cases can be as simple as follow:
            var assignment = _provider.GetSelectedElement<IReferenceExpression>(true, true);
            if (assignment != null)
            {
                return assignment.GetText().StartsWith("Visibility");

                //                    return true;
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
                     new MyBulbItem(_provider)
                   };
                }
                return _items;
            }
        }
    }

    public class MyBulbItem : BulbActionBase
    {
        private readonly ICSharpContextActionDataProvider _provider;

        public MyBulbItem(ICSharpContextActionDataProvider provider)
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
            var assignment = _provider.GetSelectedElement<IAssignmentExpression>(true, true);
            if (assignment != null)
            {
                var factory = CSharpElementFactory.GetInstance(_provider.PsiModule);
                ICSharpExpression exp;

                if(assignment.Source.GetText().EndsWith("Visible"))
                {
                    exp = factory.CreateExpressionAsIs("Visibility.Collapsed");
                }
                else
                {
                    exp = factory.CreateExpressionAsIs("Visibility.Visible");
                }

                assignment.SetSource(exp);
            }
            return null;
        }

    }
}
