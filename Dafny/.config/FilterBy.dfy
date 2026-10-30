

// REMEMBER----
// >> dotnet new tool-manifest
// >> dotnet tool install dafny

module {:extern "Bison"} BisonMod {
  class {:extern "Taxon"} Taxon {
    function {:extern "IsSubTaxon"} isSubTaxon(ancestor: Taxon): bool
  }

  class {:extern "Observation"} Observation {
    function {:extern "GetTaxon"} getTaxon(): Taxon
  }

  function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>
}

module FilterByModule {
    import B = BisonMod

    function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>
    {
        var ob := [];
        var i := 0;

        while i < |obs|
            invariant 0 <= i
            {
                if obs[i].getTaxon().isSubTaxon() 
                {
                    ob = ob + obs[i];
                }
                i = i + 1;

            }
    }

}




method FilterBy(taxon: Taxon) returns (observations: List<Observation>)
ensures observations != null
{
    
    //if (taxon.getChildren != null)
    // for (obsservations.length)
    //      if observation.getTaxon == taxon
    //      else check if observations.getTaxon == taxon.getSub
    //else 
    // for (obsservations.length)
    // check observation.getTaxon == taxon
    // return a new sequence containing those observations who have the input taxon
     
}