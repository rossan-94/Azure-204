using Azure.Storage.Sas;

namespace Rossan.Azure.BlobStore
{
    public class PermissionBuilder
    {
        /// <summary>
        /// Converts string permissions to the permission flags
        /// </summary>
        /// <param name="permissions">string permissions for accessing the storage</param>
        /// <returns>A <see cref="SharedAccessBlobPermissions"/> object that relates to the permissions</returns>
        public static BlobContainerSasPermissions GetPermissions(string permissions)
        {
            if (permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }
            var tempPermission = default(BlobContainerSasPermissions);
            var perm = permissions.Split('|');
            foreach (var permission in perm)
            {
                switch (permission.ToLower().Trim())
                {
                    case "r":
                        tempPermission |= BlobContainerSasPermissions.Read;
                        break;

                    case "w":
                        tempPermission |= BlobContainerSasPermissions.Write;
                        break;

                    case "l":
                        tempPermission |= BlobContainerSasPermissions.List;
                        break;

                    case "d":
                        tempPermission |= BlobContainerSasPermissions.Delete;
                        break;

                    case "c":
                        tempPermission |= BlobContainerSasPermissions.Create;
                        break;

                    case "a":
                        tempPermission |= BlobContainerSasPermissions.Add;
                        break;

                    default: throw new NotSupportedException("Permission Not Supported");
                }
            }
            return tempPermission;
        }
    }
}
