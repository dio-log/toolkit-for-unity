
namespace Features.Binding.Scripts
{
    public static class BindingOperation
    {
        public static void Bind<TSource, TTarget>(BindingContext<TSource, TTarget> context)
        {
            context.Source.OnValueChanged += context.OnSourceChanged;
            if (context.BindingType == BindingType.TwoWay)
            {
                context.Target.OnValueChanged += context.OnTargetChanged;
            }

            // context.Target.SetValue(context.Converter.ToTarget(context.Source.Value));
        }
        

        public static void Unbind<TSource, TTarget>(BindingContext<TSource, TTarget> context)
        {
            context.Source.OnValueChanged -= context.OnSourceChanged;
            
            if (context.BindingType == BindingType.TwoWay)
            {
                context.Target.OnValueChanged -= context.OnTargetChanged;
            }
        }

      
        
    }
}