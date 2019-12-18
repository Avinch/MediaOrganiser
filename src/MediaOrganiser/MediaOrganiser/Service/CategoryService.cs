using System.Collections.Generic;
using System.IO;
using MediaOrganiser.Data;
using MediaOrganiser.Messages;
using MediaOrganiser.Model;
using Newtonsoft.Json;

namespace MediaOrganiser.Service
{
    public class CategoryService
    {
        private readonly DataRepository _repo;

        public CategoryService()
        {
            _repo = new DataRepository();
        }

        public void LoadCategoriesIntoMemory()
        {
            if (!File.Exists(SettingsService.Instance.GetFilesPath()))
            {
                return;
            }

            var serialized = File.ReadAllText(SettingsService.Instance.GetFilesPath());

            var filesDto = JsonConvert.DeserializeObject<List<MediaFileDto>>(serialized);

            foreach (var dto in filesDto)
            {
                switch (dto.FileTypeId)
                {
                    case (int) DataEnums.FileType.Audio:
                    {
                        var file = _repo.SelectAudioFileByPath(dto.Path);

                        SetCategoriesFromDto(file, dto);

                        break;
                    }
                    case (int)DataEnums.FileType.Video:
                    {
                        var file = _repo.SelectVideoFileByPath(dto.Path);
                        
                        SetCategoriesFromDto(file, dto);

                        break;
                    }
                }
            }

            MessengerService.Default.Send(new FileCategoriesUpdatedMessage(), MessageContexts.FileCategoriesUpdatedMessage);
        }

        private void SetCategoriesFromDto(MediaFile file, MediaFileDto dto)
        {
            if (file == null)
            {
                return;
            }

            file.Categories = dto.Categories ?? new List<string>();
        }

        public void SaveCategoriesToFile()
        {
            var allDtos = new List<MediaFileDto>();

            foreach (var file in _repo.SelectAllAudioFiles())
            {
                allDtos.Add(new MediaFileDto
                {
                    Path = file.Path,
                    FileTypeId = (int)DataEnums.FileType.Audio,
                    Categories = file.Categories
                });
            }

            foreach (var file in _repo.SelectAllVideoFiles())
            {
                allDtos.Add(new MediaFileDto
                {
                    Path = file.Path,
                    FileTypeId = (int)DataEnums.FileType.Video,
                    Categories = file.Categories
                });
            }

            var serializedDtos = JsonConvert.SerializeObject(allDtos, Formatting.Indented);

            if (!Directory.Exists(SettingsService.Instance.GetSettingsFolder()))
            {
                Directory.CreateDirectory(SettingsService.Instance.GetSettingsFolder());
            }

            File.WriteAllText(SettingsService.Instance.GetFilesPath(), serializedDtos);
        }
    }
}
