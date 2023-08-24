namespace Fitness.Domain
{
    using Fitness.Domain.Seedwork;
    using System.Collections.Generic;

    public class FileExtension : Enumeration
    {
        public FileExtension() : base() { }

        public FileExtension(int id, string name, List<byte[]> signatures) : base(id, name)
        {
            this.Signatures = signatures;
        }

        public List<byte[]> Signatures { get; } = default!;

        private static readonly FileExtension gif = new(
            1,
            ".gif",
            new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } }
        );

        private static readonly FileExtension png = new(
            2,
            ",png",
            new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } }
        );

        private static readonly FileExtension jpeg = new(
            3,
            ".jpeg",
            new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
            }
        );

        private static readonly FileExtension jpg = new(
            4,
            ".jpg",
            new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
            }
        );

        public static FileExtension Gif { get; } = gif;

        public static FileExtension Png { get; } = png;

        public static FileExtension Jpeg { get; } = jpeg;

        public static FileExtension Jpg { get; } = jpg;
    }
}
