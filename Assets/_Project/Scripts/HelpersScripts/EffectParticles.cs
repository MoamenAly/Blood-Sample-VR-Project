using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParticles : MonoBehaviour
{
    [SerializeField] Color defaultColor;
    [SerializeField] Color correctAnswerColor;
    [SerializeField] Color wrongAnswerColor;
    private ParticleSystem particle;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void ChangeColorCorrectParticle()
    {
        particle.startColor = correctAnswerColor;
    }

    
    public void ChangeColorWrongParticle()
    {
        particle.startColor = wrongAnswerColor;
    }

    public void ChangeColorDefaultParticle()
    {
        particle.startColor = defaultColor;
    }
}
