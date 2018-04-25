using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public interface IVM
    {
        Security Active { get; }
    }
}