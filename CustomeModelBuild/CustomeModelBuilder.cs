using Azure.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebAPI5
{
    public class CustomeModelBuilder :IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {

            var idValueProviderResult = bindingContext.ValueProvider.GetValue("id");
            if (int.TryParse(idValueProviderResult.FirstValue, out var id))
            {
                var article = new Book { 
                    Id = id,
                    Title = bindingContext.HttpContext.Request.Query["Title"].ToString(),
                    Author = bindingContext.HttpContext.Request.Query["Author"].ToString()

                };
                bindingContext.Result = ModelBindingResult.Success(article);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }
    }
}
