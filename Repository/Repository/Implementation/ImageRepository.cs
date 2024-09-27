using Repository.DataAccessObject;
using Repository.ModelEntity;
using Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.Implementation
{
    public class ImageRepository(VietWayDbContext vietWayDbContext): GenericRepository<Image>(vietWayDbContext), IImageRepository
    {
    }
}
