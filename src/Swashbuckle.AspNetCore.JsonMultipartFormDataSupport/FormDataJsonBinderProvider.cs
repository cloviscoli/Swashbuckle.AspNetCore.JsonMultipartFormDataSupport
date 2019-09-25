﻿using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport {
	/// <summary>
	/// Looks for field with <see cref="FromJsonAttribute"/> and use <see cref="JsonModelBinder"/> for binder.
	/// </summary>
	public class FormDataJsonBinderProvider : IModelBinderProvider {
		/// <inheritdoc />
		public IModelBinder GetBinder(ModelBinderProviderContext context) {
			if (context == null) throw new ArgumentNullException(nameof(context));

			// Do not use this provider for binding simple values
			if (!context.Metadata.IsComplexType) return null;

			// Do not use this provider if the binding target is not a property
			var propName = context.Metadata.PropertyName;
			var propInfo = context.Metadata.ContainerType?.GetProperty(propName);
			if (propName == null || propInfo == null) return null;

			// Do not use this provider if the target property type implements IFormFile
			if (propInfo.PropertyType.IsAssignableFrom(typeof(IFormFile))) return null;

			// Do not use this provider if this property does not have the FromForm attribute
			if (propInfo.GetCustomAttribute<FromJsonAttribute>() == null) return null;

			// All criteria met; use the FormDataJsonBinder
			return new JsonModelBinder();
		}
	}
}