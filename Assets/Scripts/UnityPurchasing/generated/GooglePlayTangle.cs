// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("YxFcO4ZDgjgE+BUMZPR/Mfv0hn5jcQyJ+Hh5I3je2y49IJ4eIIZqxhN1APQIQrUSjSXmenNpGxsgj0Prw6ilvQLWRJhtFrqvkVeA6gEzYAfI38WLbwlNX1ef35HmldLgJPTtSzoES/ejMipcYBRXq4/BgTnoaAD2ey5XE7Uw5C15A4+n/5RjSQ8sai0WNwGF4SIKoVl+MRQw+xC5g/Kg9zwI87NV3WNDeRwBMpacn5q8gPGdraczMtvF3Nsdbyak6MkjRDpbnQEZqygLGSQvIAOvYa/eJCgoKCwpKmfmCArIsz+5LMHTDESSZCYofpMOZBmMmcTvxay3mq2kKzUHuD4zkeirKCYpGasoIyurKCgp58lFfmtOjREI3Dv/PQNZpisqKCko");
        private static int[] order = new int[] { 12,4,10,9,5,13,11,8,9,9,12,12,12,13,14 };
        private static int key = 41;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
