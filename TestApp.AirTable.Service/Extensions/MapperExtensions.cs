﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.LoggingProxy.Contracts.Exceptions;

namespace TestApp.AirTable.Services.Extensions
{
    public static class MapperExtensions
    {
        public static TT SafeMap<T, TT>(this IMapper mapper, T source, ILogger logger = null)
        {
            try
            {
                return mapper.Map<TT>(source);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, $"could not map {nameof(T)} to {nameof(TT)}");
                throw new TypeMappingException(ex);
            }
        }
    }
}
