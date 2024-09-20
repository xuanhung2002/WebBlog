using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Infrastructure.Persistance.Constants
{
    internal static class TableNames
    {
        internal const string AppUsers = nameof(AppUsers);
        internal const string AppRoles = nameof(AppRoles);

        internal const string AppUserRoles = nameof(AppUserRoles);
        internal const string AppUserClaims = nameof(AppUserClaims);
        internal const string AppUserLogins = nameof(AppUserLogins);
        internal const string AppUserTokens = nameof(AppUserTokens);
    }
}
