using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.DTOs.Admin;

namespace TMFApplication.Contracts.Admin.Queries
{
    public class GetStaffsQuery : IRequest<GetStaffsResponse>
    {
    }
}
