using AutoMapper;
using LastFMExtractor.Domain.Entities;
using LastFMExtractor.Domain.Models;

namespace LastFMExtractor
{
    public class DataMapper : Profile
    {
        public DataMapper()
        {
            CreateMap<Track, ExportedTracks>()
                .ForMember(dest => dest.DateCreatedUnix, opts => opts.MapFrom(src => src.Date.Uts))
                .ForMember(dest => dest.Track, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.TrackId, opts => opts.MapFrom(src => src.Mbid))
                .ForMember(dest => dest.Artist, opts => opts.MapFrom(src => src.Artist.Name))
                .ForMember(dest => dest.ArtistId, opts => opts.MapFrom(src => src.Artist.Mbid))
                .ForMember(dest => dest.Album, opts => opts.MapFrom(src => src.Album.Name))
                .ForMember(dest => dest.AlbumId, opts => opts.MapFrom(src => src.Album.Mbid));
        }
    }
}
