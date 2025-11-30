using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Eaship.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            var account = new Account(
                "dyjbyzk3v",                 
                "148645464431275",           
                "RKD7yC8i6mTGMx9Ei044cKNDSZE" 
            );

            _cloudinary = new Cloudinary(account);
        }

        public string UploadImage(string filePath)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath)
            };

            var result = _cloudinary.Upload(uploadParams);

            return result.Url.ToString(); 
        }
    }
}
