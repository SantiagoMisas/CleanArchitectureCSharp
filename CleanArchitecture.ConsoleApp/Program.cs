using CleanArchitecture.Data;
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Linq;

StreamerDbContext dbContext = new();

//await AddNewRecords();
//QueryStreaming();
//await QueryMethods();
//await QueryLinq();
await TrackingAndNotTracking();

async Task TrackingAndNotTracking() { 

    var streamerWithTracking = await dbContext!.Streamers!.FirstOrDefaultAsync(x=> x.Id == 1);
    var streamerWithNoTracking = await dbContext!.Streamers!.AsNoTracking().FirstOrDefaultAsync(x => x.Id == 2);

    streamerWithTracking.Nombre = "Netflix Super";
    streamerWithNoTracking.Nombre = "Amazon Plus";

    dbContext!.SaveChangesAsync();
}

async Task QueryLinq() {

    Console.WriteLine($"Ingrese una compania de streaming");
    var streamerNombre = Console.ReadLine();

    var streamers = await (from i in dbContext.Streamers 
                           where EF.Functions.Like(i.Nombre, $"%{streamerNombre}%") 
                           select i).ToListAsync();

    foreach (var streamer in streamers) {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
}


Console.WriteLine("Presione cualquier tecla para terminar el programa");
Console.ReadKey();

async Task QueryMethods() {

    var streamer = dbContext!.Streamers!;
    var firstAsync = await streamer.Where(y => y.Nombre.Contains("a")).FirstAsync();
    var firstOrDefaulAsync = await streamer.Where(y => y.Nombre.Contains("a")).FirstOrDefaultAsync();
    var firstOrDfaultV2 = await streamer.FirstOrDefaultAsync(y => y.Nombre.Contains("a"));
    var singleAsync = await streamer.Where(y => y.Id == 1).SingleAsync();
    var singleOrDefaultAsync = streamer.Where(y => y.Id == 1).SingleOrDefaultAsync();
    var resultado= await streamer.FindAsync(1);
}


async Task QueryFilter() {

    Console.WriteLine("Ingrese una compania de streaming");
    var streamingNombre = Console.ReadLine();
    var streamers = await dbContext!.Streamers!.Where(x => x.Nombre.Equals(streamingNombre)).ToListAsync();
    foreach (var streamer in streamers) { 
    Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }

    //var streamerPartialResults = await dbContext!.Streamers!.Where(x=> x.Nombre.Contains(streamingNombre)).ToListAsync();
    var streamerPartialResults = await dbContext!.Streamers!.Where(x => EF.Functions.Like(x.Nombre, $"%{streamingNombre}%" )).ToListAsync();
    foreach (var streamer in streamerPartialResults)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
}

void QueryStreaming() { 

    var streamers = dbContext!.Streamers!.ToList();
    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
}

async Task AddNewRecords() {
    Streamer streamer = new()
    {
        Nombre =  "Disney",
        Url = "https://www.disney.com"
    };

    dbContext!.Streamers!.Add(streamer);

    await dbContext.SaveChangesAsync();

    var movies = new List<Video>
    {
        new Video {
        Nombre = "Cenicienta",
        StreamerId = streamer.Id
        },
        new Video {
        Nombre = "1001 Dalmatas",
        StreamerId = streamer.Id
        },
        new Video {
        Nombre = "El Jorobado De Notredame",
        StreamerId = streamer.Id
        },
        new Video {
        Nombre = "Star Wars",
        StreamerId = streamer.Id
        }
    };
    await dbContext.AddRangeAsync(movies);

    await dbContext.SaveChangesAsync();
}