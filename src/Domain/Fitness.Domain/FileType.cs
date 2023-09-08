namespace Fitness.Domain
{
    using Fitness.Domain.Seedwork;
    using System;

    public class FileType : Enumeration
    {
        public FileType() : base() { }

        public FileType(int id, string name) : base(id, name)
        {
        }

        private static readonly FileType video = new(1, "videos");

        private static readonly FileType picture = new(2, "pictures");

        public static FileType Video { get; } = video;

        public static FileType Picture { get; } = picture;
    }
}
