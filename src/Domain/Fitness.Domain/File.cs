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
            long fileLenght,
            FileType fileType,
            Guid entityId,
            bool isDeleted,
            DateTimeOffset createdAt)
        {
            this.Id = id;
            this.Name = name;
            this.FileExtensionId = fileExtension.Id;
            this.FileLength = fileLenght;
            this.FileTypeId = fileType.Id;
            this.EntityId = entityId;
            this.IsDeleted = isDeleted;
            this.CreatedAt = createdAt;
        }

        public Guid Id { get; }

        public string Name { get; } = default!;

        public int FileExtensionId { get; } = default;

        public long FileLength { get; } = default!;

        public int FileTypeId { get; }

        public Guid EntityId { get; }

        public bool IsDeleted { get; }

        public DateTimeOffset CreatedAt { get; }
    }
}
