﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaOrganiser.Data;
using MediaOrganiser.Messages;
using MediaOrganiser.Model;
using Newtonsoft.Json;

namespace MediaOrganiser.Service
{
    public class PlaylistService
    {
        private readonly DataRepository _repo;

        public PlaylistService()
        {
            _repo = new DataRepository();
        }

        public void LoadPlaylistsIntoMemory()
        {
            if (!File.Exists(SettingsService.Instance.GetPlaylistFilePath()))
            {
                return;
            }

            var serialized = File.ReadAllText(SettingsService.Instance.GetPlaylistFilePath());

            var playlistDtos =
                JsonConvert.DeserializeObject<List<PlaylistDto>>(serialized);

            foreach (var dto in playlistDtos)
            {
                if ((DataEnums.PlaylistType) dto.TypeId == DataEnums.PlaylistType.Audio)
                {
                    var playlist = new Playlist<AudioFile>(dto.Id, dto.Name);

                    foreach (var path in dto.FilePaths)
                    {
                        playlist.Items.Add(_repo.SelectAudioFileByPath(path));
                    }

                    _repo.AddAudioPlaylist(playlist);
                }
                else if ((DataEnums.PlaylistType) dto.TypeId == DataEnums.PlaylistType.Video)
                {
                    var playlist = new Playlist<VideoFile>(dto.Id, dto.Name);

                    foreach (var path in dto.FilePaths)
                    {
                        playlist.Items.Add(_repo.SelectVideoFileByPath(path));
                    }
                    _repo.AddVideoPlaylist(playlist);
                }
            }

            MessengerService.Default.Send(new PlaylistsLoadedMessage(), MessageContexts.PopulateAudioPlaylists);


            MessengerService.Default.Send(new PlaylistsLoadedMessage(), MessageContexts.PopulateVideoPlaylists);
        }

        public void SavePlaylistsToFile()
        {
            var allDtos = new List<PlaylistDto>();

            foreach (var audioPlaylist in _repo.SelectAllAudioPlaylists())
            {
                var dto = new PlaylistDto
                {
                    Id = audioPlaylist.Id,
                    Name = audioPlaylist.Name,
                    TypeId = (int)DataEnums.PlaylistType.Audio,
                    FilePaths = audioPlaylist.Items.Select(x => x.Path).ToList()
                };

                allDtos.Add(dto);
            }

            foreach (var videoPlaylists in _repo.SelectAllVideoPlaylists())
            {
                var dto = new PlaylistDto
                {
                    Id = videoPlaylists.Id,
                    Name = videoPlaylists.Name,
                    TypeId = (int)DataEnums.PlaylistType.Video,
                    FilePaths = videoPlaylists.Items.Select(x => x.Path).ToList()
                };

                allDtos.Add(dto);
            }

            var serializedDtos = JsonConvert.SerializeObject(allDtos);

            if (!Directory.Exists(SettingsService.Instance.GetSettingsFolder()))
            {
                Directory.CreateDirectory(SettingsService.Instance.GetSettingsFolder());
            }

            File.WriteAllText(SettingsService.Instance.GetPlaylistFilePath(), serializedDtos);

            // todo: check if file in use already
        }
    }
}
