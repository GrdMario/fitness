namespace Fitness.Domain
{
    using System;

    public class File
    {
        private File() { }

        public File(
            Guid id,
            string name,
            FileExtension fileExtension,
            byte[] data,
            FileType fileType,
            Guid entityId,
            bool isDeleted,
            DateTimeOffset createdAt)
        {
            this.Id = id;
            this.Name = name;
            this.FileExtension = fileExtension;
            this.Data = data;
            this.FileType = fileType;
            this.EntityId = entityId;
            this.IsDeleted = isDeleted;
            this.CreatedAt = createdAt;
        }

        public Guid Id { get; }

        public string Name { get; } = default!;

        public int FileExtensionId { get; } = default;

        public FileExtension FileExtension { get; } = default!;

        public byte[] Data { get; } = default!;

        public int FileTypeId { get; }

        public FileType FileType { get; } = default!;

        public Guid EntityId { get; }

        public bool IsDeleted { get; }

        public DateTimeOffset CreatedAt { get; }
    }
}
