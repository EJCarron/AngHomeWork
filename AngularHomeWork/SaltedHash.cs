using System;
using System.Security.Cryptography;
using System.Text;
namespace AngularHomeWork {
    public class SaltedHash {
        public string salt;
        public string saltedHash;

        public SaltedHash(string password) {

            this.salt = getSalt();

            string saltedPassword = salt + password;

            this.saltedHash = getSaltedHash(saltedPassword);

        }

        public void renderSalt() {
            Console.WriteLine(this.salt);
        }

        public void renderSaltedHash() {
            Console.WriteLine(this.saltedHash);
        }

        private static string getSaltedHash(string saltedPassword) {

            SHA256 sha = SHA256Managed.Create();
            //turn SaltedPassword into byte array
            byte[] bytes = Encoding.UTF8.GetBytes(saltedPassword);
            // hash the array of bites
            byte[] hash = sha.ComputeHash(bytes);
            return GetStringFromHash(hash);


        }

        private static string GetStringFromHash(byte[] hash) {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        public string getSalt() {

            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider()) {

                byte[] data = new byte[4];

                string newSalt = "";
                for (int i = 0; i < 10; i++) {

                    rngCsp.GetBytes(data);


                    int value = BitConverter.ToInt32(data, 0);

                    if (value < 0) {
                        value = value * -1;
                    }

                    newSalt += value.ToString();

                }

                return newSalt;

            }

        }

        public static bool isPasswordCorrect(string passwordAttempt, string salt, string saltedHash) {


            string saltedPassword = salt + passwordAttempt;

            string attamptedSaltedHash = getSaltedHash(saltedPassword);

            if (attamptedSaltedHash == saltedHash) {
                return true;
            } else {
                return false;
            }

        }
    }
}
