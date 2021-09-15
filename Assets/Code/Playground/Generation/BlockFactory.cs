using Playground.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Playground.Generation
{
    public class BlockFactory : MonoBehaviour, IService
    {
        [SerializeField]
        private ServiceCollector serviceCollector;

        private Dictionary<Type, BlockMetadata> blockMetadata;
        public IReadOnlyDictionary<Type, BlockMetadata> BlockMetadata => blockMetadata;

        private void Awake()
        {
            blockMetadata = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(asm => asm.GetTypes())
                            .Where(type => type.IsSubclassOf(typeof(Block)))
                            .Select(lb => new BlockMetadata(lb))
                            .ToDictionary(lb => lb.BlockType, lb => lb);
        }

        public T CreateLogicBlock<T>() where T : Block
        {
            if (blockMetadata.TryGetValue(typeof(T), out var lbm))
            {
                return (T)CreateLogicBlock(lbm);
            }
            return null;
        }

        public Block CreateLogicBlock(BlockMetadata lbm)
        {
            return CreateLogicBlock(lbm.BlockType);
        }

        public Block CreateLogicBlock(Type type)
        {
            var block = (Block)Activator.CreateInstance(type);
            block.Prepare(serviceCollector);
            return block;
        }
    }
}
