namespace Fitness.Domain
{
    using Fitness.Domain.Seedwork;
    using System;

    public class FileType : Enumeration
    {
        public FileType() : base() { }

        public FileType(int id, string name, string path) : base(id, name)
        {
            this.Path = path;
        }

        public string Path { get; } = default!;

        public string ResolvedPath(Guid entityId, Guid fileId)
            => this.Path.Replace("ENTITY-ID", entityId.ToString()).Replace("FILE_ID", fileId.ToString());

        private static readonly FileType exerciseVideo = new(1, "Exercise Video", "/fitness/[ENTITY_ID]/video/[FILE_ID]");

        private static readonly FileType profilePicture = new(2, "Profile Picture", "/profile/[ENTITY-ID]/img/[FILE_ID]");

        public static FileType ExerciseVideo { get; } = exerciseVideo;

        public static FileType ProfilePicture { get; } = profilePicture;
    }
}
