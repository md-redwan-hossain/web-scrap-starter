using RokomariScrap;
using RokomariScrap.Utils;


var sqlitePath = PathUtils.TopLevelSqlitePath(Directory.GetCurrentDirectory());
var connectionString = $"Data Source={sqlitePath};Cache=Shared";

var databaseUtils = new DatabaseUtils(connectionString);
await databaseUtils.Reset();
await databaseUtils.LoadMigration();


const uint pageNumberLowerLimit = 1;
const uint pageNumberHigherLimit = 5;

for (var pageNumber = pageNumberLowerLimit; pageNumber <= pageNumberHigherLimit; pageNumber++)
{
    var data = await Scrap.Act(pageNumber);
    await using var db = new DatabaseWriter(new AppDbContext(connectionString), data);
    await db.Write();
}