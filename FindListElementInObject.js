db.Test2.find(
   {
      "SeriesName" : "Mistborn"
      
   },
   {_id: 0, Books: {$elemMatch: {Name: "Hero of Ages"}}}
   )
