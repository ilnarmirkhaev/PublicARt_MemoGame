using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using ViewControllers;
using Random = UnityEngine.Random;

namespace Cards
{
    public class CardsLayout : MonoBehaviour
    {
        [SerializeField] private List<Texture> allTextures;
        [SerializeField] private GameObject cardPool;
        
        private List<Card> _cards = new();
        private Dictionary<Texture, int> _currentTextures = new();

        private void Awake()
        {
            GetCards();
        }

        private void OnEnable()
        {
            StartMenuViewController.OnGameEntered += ActivateCards;
            SinglePlayerViewController.OnGameStarted += LayoutCards;
            SinglePlayerViewController.OnGameExited += DeactivateCards;
            
            MultiPlayerViewController.OnGameStarted += LayoutCards;
            MultiPlayerViewController.OnGameExited += DeactivateCards;
        }

        private async void LayoutCards()
        { 
            var tasks = _cards.Select(card => card.ResetCard()).ToList();
            await Task.WhenAll(tasks);
            
            _currentTextures = RandomTextures(allTextures, 8);
            SetCardTextures(_cards, _currentTextures);
        }

        private void OnDisable()
        {
            StartMenuViewController.OnGameEntered -= ActivateCards;
            SinglePlayerViewController.OnGameStarted -= LayoutCards;
            SinglePlayerViewController.OnGameExited -= DeactivateCards;
            
            MultiPlayerViewController.OnGameStarted -= LayoutCards;
            MultiPlayerViewController.OnGameExited -= DeactivateCards;
        }
        
        private void ActivateCards() => cardPool.SetActive(true);
        private void DeactivateCards() => cardPool.SetActive(false);

        private void GetCards()
        {
            _cards = cardPool.GetComponentsInChildren<Card>().ToList();
        }

        private static Dictionary<Texture, int> RandomTextures(IList<Texture> allTextures, int count)
        {
            var textures = allTextures.ToList();
            var pickedTextures = new Dictionary<Texture, int>();
            
            for (var i = 0; i < count; i++)
            {
                var texture = textures[Random.Range(0, textures.Count)];
                pickedTextures.Add(texture, 2);
                
                textures.Remove(texture);
            }

            return pickedTextures;
        }

        private static void SetCardTextures(List<Card> cards, Dictionary<Texture, int> texturesDictionary)
        {
            var textureKeys = texturesDictionary.Keys.ToList();
            
            foreach (var card in cards)
            {
                var texture = textureKeys[Random.Range(0, textureKeys.Count)];

                texturesDictionary[texture]--;
                if (texturesDictionary[texture] == 0)
                    textureKeys.Remove(texture);
                
                card.SetTexture(texture);
            }
        }
    }
}