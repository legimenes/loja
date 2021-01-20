using System;
using Loja.Core.Identity.Managers;
using Loja.Core.Identity.Stores;
using Moq;

namespace Loja.Tests.Core.Identity.Fixtures
{
    public class RsaFixture : IDisposable
    {
        public readonly Mock<IRsaStore> MockedRsaStore;
        public readonly IRsaManager RsaManager;

        public RsaFixture()
        {
            MockedRsaStore = new Mock<IRsaStore>();
            RsaManager = new RsaManager(MockedRsaStore.Object);
        }

        public string GetPrivateKey()
        {
            string privateKey = 
                "-----BEGIN PRIVATE KEY-----\n" +
                "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDpq7tf5pl2pkZk\n" +
                "zQM3EEkMNcG8F0WmenQ/Vz/g89b1W8iidaMLzJfvRvKsv29O4xWaQiCg6Ge3HLxD\n" +
                "IrmElOwgXTkGmbZTl1JaZio0YNJ4KSRLBMb1yGs7z6akmB8pgqoCLatwSIp55oS+\n" +
                "rgUnYUHpDJ62/Dl93+QxQzA/dH/v7/CgZOBX+InKouLjlA5h9m6cOfvxXZA1lGbv\n" +
                "iujbOJEZDyWvHd2Ke0D9vc1NezYII4W4iXFIlNSaJJMxPqMibwf45MgHKIHp9mE9\n" +
                "jF3b2TMcMYLFyyNOJiBZxUsA/yOMS7YF1i4kTOeu9i7HzC/4E0KPLgHPY0DC3/tV\n" +
                "l3yknpX9AgMBAAECggEAG63G9TYoiYBqqDgMoHIiZPrdJv2Ot4ibyoD4RRo9JTUp\n" +
                "D+1lUdl7SdGan1HRyoNlpK8elFDTMEKMHlu3o/fL/I1uWtTMXxx2mdSuPSQW6jrd\n" +
                "XLGFK8oAwcX4FFkcn9slbjqgqx08ofHnWXrdi2ue1g8zobNA0CM8MYWm6m1PVklY\n" +
                "tLUJoGYIaxHkye7FwaMBGMXl8Qko9Cf2p+sNDZmwBxGdwYVyu4yuuyTU6P3xWL9Q\n" +
                "JdAN7DFt6H1bGsu2vUKIJV9A8JYSlMjh5aggjvEmy/2PaK7YgbyHM1APUXZzMU3S\n" +
                "HnRvaitl8BX5GKuOSCixsz26K8drFv8S/ZGRfP6E4QKBgQD/kO5ZaX2wzC5d20Hh\n" +
                "PW30ylPqlgHqm7dwpggCNacJpIVwL3bA73vVnZPODZPtSjAL+8l3lG95zZf6MBvm\n" +
                "jwPiLH1GL0whMTfq2PpJTB3mMbIBU8R5h6az9EOmIO8AWuJwaUee4XALiB/D8BQ9\n" +
                "9iv1+ALa0auWttCfMQrEVod9lQKBgQDqEUkCBSVxgklj9lzv5BqjcbSDS1xfaml7\n" +
                "plng65ZKQg/dbd8ZJJjJM2VSIGHhmbAKiO+8GDgl8I0jgKnbxd1EeF2rdyOaPPdb\n" +
                "slzIUQThcIEsnuY1JlPc60kI6Pl18LXDtM9DwcwSN8poYZLZDaTjZWzcnIPULY8l\n" +
                "YT3nNnIMyQKBgQDjOVwCGV4MfG3ZOyGm/vQtilr0Hu2TR2HhAW3rcQKT+zg9F6ZR\n" +
                "QlxrAFCzCrV0a9quPO7SqCI5PMecRXv5ET0VshKr/U+Fz3n1D3fxBYEr8xFeRrlQ\n" +
                "iIB6TXp8UZnOSgA8jA6Gv8/cIOqFTobg1GgfqKP5JCSYuvBgKb119a0/xQKBgQCo\n" +
                "Z6dPfMRj1olXEnnrXwKLddOaYy4iuD0MabNg0B9hbgZcGiDZxirnF8NeQ04pMpol\n" +
                "+kAB5KsBIQFq+bc8GDAKg09hfmZvIk4V+04mEaShToChyfF3bAwKdn4lmvlgkb80\n" +
                "/3HgHh7lPJ60Wv98iwSHVwHr9/AhSGYlTsFrCRElgQKBgEIC7cSw3+RvbJFqD7LB\n" +
                "4aC0PWU26bQkvPLARCPg8uTt60yDU8iEAVfFXU3a20SHx9/qoplrgNt5c6ZCURE2\n" +
                "Ntg96b2LYRHBUdfFo4uyTaPzzGJGKH5/u6r0s0vAttIbv2ZSmtILmOY+rjQkgL4x\n" +
                "8wLqQCpWul0u+6YsTrAyoDge\n" +
                "-----END PRIVATE KEY-----";

            return privateKey;
        }

        public string GetPublicKey()
        {
            string publicKey =
                "-----BEGIN PUBLIC KEY-----\n" +
                "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA6au7X+aZdqZGZM0DNxBJ\n" +
                "DDXBvBdFpnp0P1c/4PPW9VvIonWjC8yX70byrL9vTuMVmkIgoOhntxy8QyK5hJTs\n" +
                "IF05Bpm2U5dSWmYqNGDSeCkkSwTG9chrO8+mpJgfKYKqAi2rcEiKeeaEvq4FJ2FB\n" +
                "6Qyetvw5fd/kMUMwP3R/7+/woGTgV/iJyqLi45QOYfZunDn78V2QNZRm74ro2ziR\n" +
                "GQ8lrx3dintA/b3NTXs2CCOFuIlxSJTUmiSTMT6jIm8H+OTIByiB6fZhPYxd29kz\n" +
                "HDGCxcsjTiYgWcVLAP8jjEu2BdYuJEznrvYux8wv+BNCjy4Bz2NAwt/7VZd8pJ6V\n" +
                "/QIDAQAB\n" +
                "-----END PUBLIC KEY-----";

            return publicKey;
        }

        public void Dispose() { }
    }
}