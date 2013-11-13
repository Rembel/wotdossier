using System;
using System.Collections.Generic;

namespace WotDossier.Applications.Logic.Export
{
    public abstract class ExportProviderBase<T> : IExportProviderBase<T>
    {
        public abstract void Export(List<T> list, List<Type> exportInterfaces);
    }
}