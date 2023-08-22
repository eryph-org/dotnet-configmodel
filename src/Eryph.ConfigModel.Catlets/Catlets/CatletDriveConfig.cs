using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class CatletDriveConfig: IMutateableConfig<CatletDriveConfig>, ICloneable
    {
        public string? Name { get; set; }
        
        public MutationType? Mutation { get; set; }
        
        public string? Location { get; set; }
        public string? Store { get; set; }

        [PrivateIdentifier]
        public string? Source { get; set; }

        public int? Size { get; set; }
        public CatletDriveType? Type { get; set; }

        public CatletDriveConfig Clone()
        {
            return new CatletDriveConfig
            {
                Name = Name,
                Mutation = Mutation,
                Location = Location,
                Store = Store,
                Source = Source,
                Size = Size,
                Type = Type
            };
        }
        
        object ICloneable.Clone()
        {
            return Clone();
        }

        internal static CatletDriveConfig[]? Breed(CatletConfig parentConfig, CatletConfig child, 
            string? parentReference)
        {
            return Breeding.WithMutation(parentConfig, child, x => x.Drives,
                (drive, childDrive) =>
                {

                    if (childDrive.Type.HasValue && drive.Type != childDrive.Type)
                        drive.Source = null;
                    
                    drive.Type = childDrive.Type ?? drive.Type;
                    drive.Source = childDrive.Source ?? drive.Source;
                    
                    if (childDrive.Size != 0) drive.Size = childDrive.Size;
                    if (!string.IsNullOrWhiteSpace(childDrive.Location))
                        drive.Location = childDrive.Location;
                    if (!string.IsNullOrWhiteSpace(childDrive.Store))
                        drive.Store = childDrive.Store;               
                },
                (drive) =>
                {
                    if(string.IsNullOrWhiteSpace(drive.Source))
                    {
                        drive.Source = $"gene:{parentReference}:{drive.Name}";
                    }
                }
            );
            
         }
    }
}