using System.Numerics;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Zeus.InternalLogger
{
    /// <summary>
    /// Defines utility methods for <see cref="IWcfLogger"/> objects.
    /// </summary>
    internal static class IWcfLoggerUtility
    {
        #region Extension methods

        /// <summary>
        /// Extension method:
        /// Casts the <see cref="IWcfLogger"/> object to the type specified by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to wich the object needs to be casted.</typeparam>
        /// <param name="obj">The object to cast.</param>
        /// <returns>The casted object if succeed, null otherwise.</returns>
        public static T Cast<T>(this IWcfLogger obj) where T : class
        {
            return obj as T;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="ServiceEndpoint"/> that has to be used by the <see cref="IWcfLogger"/> service.
        /// </summary>
        public static ServiceEndpoint Endpoint { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the <see cref="IWcfLoggerUtility"/> properties.
        /// </summary>
        static IWcfLoggerUtility()
        {
            s_RndGen = RandomNumberGenerator.Create();
            s_Buff = new byte[c_RandomNumberSize];
            Endpoint = new WebHttpEndpoint(ContractDescription.GetContract(typeof(IWcfLogger)), new EndpointAddress("http://localhost:21385/InternalLogger"));
        }

        #endregion

        #region Constants

        /// <summary>
        /// The size in bytes of a random generated number.
        /// </summary>
        private const int c_RandomNumberSize = 32;

        #endregion

        #region Fields

        /// <summary>
        /// The random object used to generate random numbers.
        /// </summary>
        private static RandomNumberGenerator s_RndGen;
        /// <summary>
        /// The array that contains the random generated number.
        /// </summary>
        private static byte[] s_Buff;

        #endregion

        #region Methods

        /// <summary>
        /// Generates a random number.
        /// </summary>
        /// <returns>A new random number.</returns>
        public static BigInteger GenerateRandomInt()
        {
            s_RndGen.GetBytes(s_Buff);
            return BigInteger.Abs(new BigInteger(s_Buff));
        }
        /// <summary>
        /// Generates a new publick key.
        /// </summary>
        /// <param name="radix">The radix used in the public key genration process.</param>
        /// <param name="mod">The module used in the public key generation process.</param>
        /// <param name="prKey">The private key to be used in the public key generation process.</param>
        /// <returns>The public key asociated with the given parameters.</returns>
        public static BigInteger GetPublicKey(BigInteger radix, BigInteger mod, BigInteger prKey)
        {
            return BigInteger.ModPow(radix, prKey, mod);
        }

        /// <summary>
        /// Generates the shared key.
        /// </summary>
        /// <param name="mod">The module used in the shared key generation process.</param>
        /// <param name="prKey">The private key used in the shared key generation process.</param>
        /// <param name="pbKey">The public key of the other "part" used in the shared key generation process.</param>
        /// <returns>The shared key associated with the given parameters.</returns>
        public static BigInteger GetSharedKey(BigInteger mod, BigInteger prKey, BigInteger pbKey)
        {
            return BigInteger.ModPow(pbKey, prKey, mod);
        }

        #endregion
    }
}
