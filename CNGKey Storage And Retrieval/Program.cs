using System;
using System.Text;
using System.Security.Cryptography;

public class Program
{
    public static void Main()
    {

        String crypto_key_name = "key name";

        /* 
        * We Must First Specify A CNG Provider. In Essence, The CNG Provider Specifies Where 
        * Key Material Will Be Stored. In General, CNG Providers Are Platform Specific and/or 
        * Hardware Specific.
        * 
        * By Default, the .NET Framework Supports Three CNG Providers - None of Which 
        * Function on a Cross-Plaform Basis. These Include:
        * 
        * • MicrosoftSoftwareKeyStorageProvider - Stores Cryptographic Keys On The Microsoft 
        *   Windows OS File System (Located at C:\ProgramData\Microsoft\Crypto).
        * • MicrosoftPlatformCryptoProvider - Stores Cryptographic Keys On A TPM Device 
        *   (Motherboard-Level Hardware).
        * • MicrosoftSmartCardKeyStorageProvider - Stores Cryptographic Keys On A Removable 
        *   Smart Card Device.
        * 
        * In Order To Utilise Non-Microsoft Operating Systems - or Hardware Vendors - 
        * External CNGProvider Libraries Are Required. These Are Provided By The Vendor.
        * 
        * Common Third-Party Solutions Used For This Purpose Include AWS CloudHSM, Azure 
        * Dedicated HSM, GCP Cloud HSM, As Well As YubiKeys.
        * 
        */

        CngProvider key_storage_provider = CngProvider.MicrosoftSoftwareKeyStorageProvider;

        /*
        * In Order To Persist A Crytographic Key - To Whatever Storage Medium Is Associated 
        * With The Chosen Key Storage Provider - A CNGKey Object Is Required.
        * 
        * Each CNGKey Has A Unique Name Associated With It. Note That The Name Does Not Have 
        * To Be Globally Unique, Rather It Must Be Unique In The Context Of The Chosen Key 
        * Storage Provider.
        * 
        * Each CNGKey Can Only Be Created Once - As Such, The IF-Block Below First Checks If The 
        * Named CNGKey Already Exists. If The CNGKey Does Not Exist Already, A New One Is 
        * Created By Invoking The static Create() Method.
        * 
        */

        if (!CngKey.Exists(crypto_key_name, key_storage_provider))
        {
            CngKeyCreationParameters key_creation_parameters = new CngKeyCreationParameters()
            {
                Provider = key_storage_provider
            };//Note That The Above Parameters Prevent Keys Being Exported From The KSP By Default.

            CngKey.Create(new CngAlgorithm("AES"), crypto_key_name, key_creation_parameters);//When Used On .NET Framework (Outdated), Key Size May Need To Be Specified, e.g. AES-256.
        }

        /*
        * Following This, An Instance Of AesCng Is Created.
        * 
        * Within The Constructor, We Specify:
        *  • The Key Name Of The Associated CNGKey Object (AESCng Requires That The CNGKey 
        *    Already Exist - Hence The IF-Block Above).
        *  • The Associated CngProvider.
        * 
        * Given That AESCng Implements The Standard AES Interface, It Can Be Used In The Same 
        * Manner As Demonstrated Previously.
        * 
        */

        Aes aes = new AesCng(crypto_key_name, key_storage_provider);

    }

}