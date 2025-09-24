using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Reposatries;
public class PhotoRepositry : GenericRepositry<Photo>, IPhotoRepositry
{
    public PhotoRepositry(AppDbContext context) : base(context)
    {
    }
}
