using System;
using System.Linq;
using TouristRybinsk.Data;

class Program
{
    static int failures;
    static void Check(string name, Action test)
    {
        try { test(); Console.WriteLine("PASS: " + name); }
        catch (Exception error) { failures++; Console.WriteLine("FAIL: " + name + ": " + error.Message); }
    }
    static void Assert(bool condition) { if (!condition) throw new Exception("Assertion failed"); }
    static int Main()
    {
        var groups = SampleDataSource.GetGroups("AllGroups").ToList();
        var items = groups.SelectMany(group => group.Items).ToList();
        Check("catalog size is preserved", () => Assert(groups.Count == 8 && items.Count == 26));
        foreach (var entry in items)
        {
            var item = entry;
            Check("exact item lookup: " + item.UniqueId,
                () => Assert(Object.ReferenceEquals(SampleDataSource.GetItem(item.UniqueId), item)));
            Check("group ownership: " + item.UniqueId,
                () => Assert(item.UniqueId.StartsWith(item.Group.UniqueId + "-Item-", StringComparison.Ordinal)));
        }
        Check("unknown item", () => Assert(SampleDataSource.GetItem("unknown") == null));
        return failures == 0 ? 0 : 1;
    }
}
