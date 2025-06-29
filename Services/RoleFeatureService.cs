using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Repositories;

namespace Hotel_Management.Services
{
    public class RoleFeatureService
    {
        GeneralRepository<RoleFeature> _generalRepository;
        public RoleFeatureService(GeneralRepository<RoleFeature> generalRepository )
        {
            _generalRepository = generalRepository;
        }

        public async Task<bool> AssignFeatureToRoleAsync(Role role,Features feature)
        {
            var rolefeature = new RoleFeature()
            {
                Role = role,
                Feature = feature
            };
            await _generalRepository.AddAsync( rolefeature );

            return true;
        }

        public bool CheckFeatureAccess(Features feature,Role role)
        {
            return _generalRepository.Get(e=>e.Role==role && e.Feature==feature).Any();
        }
    }
}
