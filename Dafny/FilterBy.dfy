// From this directory, restore and verify with:
// `dotnet tool restore`
// `dotnet tool run dafny -- verify FilterBy.dfy --solver-path <path-to-z3.exe>`
// Generate FilterBy.cs with:
// `dotnet tool run dafny -- translate cs FilterBy.dfy --include-runtime --solver-path <path-to-z3.exe> -o FilterBy.cs`


//{:extern "Bison"} is reference to existing C# namespace (module).
module {:extern "Bison"} BisonMod {
  class {:extern "Taxon"} Taxon { //amnd yes here it references Taxon in C#
    function {:extern "isSubTaxon"} isSubTaxon(ancestor: Taxon): bool
  }

  class {:extern "Observation"} Observation {
    function {:extern "GetTaxon"} {:axiom} getTaxon(): Taxon
  }

}

module FilterByModule {
    import opened B = BisonMod //we well import above module

    predicate isValid(root: Taxon, ob: Observation) //Predicate is a boolean function

    //reads declares what objects our predicate/function is allowed to "see".
    //Dafny can NOT check our C# code to see if we ever change something on the observation object
    //buuuut we can use it to prove our DAFNY code does NOT do such changes.
    //So we kinda just assume the C# code works as intended and Taxon/Observation is not changed
    reads ob, root, ob.getTaxon() 
    {
        ob.getTaxon().isSubTaxon(root)
    }

    function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>
    //we dont think there is any precondition for this, as we simply didnt allow for observations or taxons to be null...
    ensures forall ob::ob in FilterBy(root, obs) ==> ob in obs && isValid(root, ob) //post condition
    ensures forall ob::ob in obs && isValid(root, ob) ==> ob in FilterBy(root, obs) //post condition
    decreases |obs|
    {
        if |obs| == 0 then []
        else if obs[0].getTaxon().isSubTaxon(root) then [obs[0]] + FilterBy(root, obs[1..])
        else FilterBy(root, obs[1..])
    }
}

module WrappedFilterBy {
    import opened B = BisonMod
    import opened F = FilterByModule

    method WrappedFilter(root: Taxon, obs: seq<Observation>) returns (filtered_obs: seq<Observation>) 
    {
        filtered_obs := F.FilterBy(root, obs);
        expect forall ob::ob in filtered_obs ==> F.isValid(root, ob), "Invalid observation found";
    }
}