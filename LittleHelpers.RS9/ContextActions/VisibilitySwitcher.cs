using System;
using System.Collections.Generic;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
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
                     new VisibilityBulbItem(_provider)
                   };
                }
                return _items;
            }
        }
    }

    public class VisibilityBulbItem : BulbActionBase
    {
        private readonly ICSharpContextActionDataProvider _provider;

        public VisibilityBulbItem(ICSharpContextActionDataProvider provider)
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
            var refExp = _provider.GetSelectedElement<IReferenceExpression>(true, true);

            ICSharpExpression exp = null;
            if (refExp != null)
            {
                var factory = CSharpElementFactory.GetInstance(_provider.PsiModule);

                var node = refExp.NameIdentifier.Name == "Visibility" ? refExp.Parent : refExp;

                refExp = node as IReferenceExpression;
                if (refExp != null)
                {
                    if (refExp.NameIdentifier.Name == "Visible")
                    {
                        exp = factory.CreateExpressionAsIs("Visibility.Collapsed");
                    }
                    else if (refExp.NameIdentifier.Name == "Collapsed")
                    {
                        exp = factory.CreateExpressionAsIs("Visibility.Visible");
                    }
                    if (exp != null)
                    {
                        refExp.ReplaceBy(exp);
                    }
                }
            }

            return null;
        }

    }
}
