using System;
using System.Collections.Generic;

namespace WotDossier.Applications.Logic.Export
{
    public interface IExportProviderBase<T>
    {
        void Export(List<T> list, List<Type> exportInterfaces);
    }
}
