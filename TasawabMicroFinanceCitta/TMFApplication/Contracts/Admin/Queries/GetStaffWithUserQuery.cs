using MediatR;
using TMFApplication.DTOs.Admin;

public class GetStaffWithUserQuery : IRequest<StaffWithUserResponse>
{
    public string StaffCode { get; set; }
}