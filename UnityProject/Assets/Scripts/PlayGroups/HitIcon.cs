﻿using System.Collections;
using UnityEngine;

namespace PlayGroup
{
    public class HitIcon : MonoBehaviour
    {
        private bool isFading;
        private Vector3 lerpFrom;
        private Vector3 lerpTo;
        private SpriteRenderer spriteRenderer;
        private readonly Color transparent = new Color(1f, 1f, 1f, 0f);
        private readonly Color visible = new Color(1f, 1f, 1f, 1f);

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ShowHitIcon(Vector2 dir, Sprite sprite)
        {
            if (isFading)
            {
                return;
            }

            var newDir = new Vector3(dir.x, dir.y, 0f);
            lerpFrom = newDir * 0.75f;
            lerpTo = newDir;
            isFading = true;
            spriteRenderer.sprite = sprite;

            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(FadeIcon());
            }
        }

        private IEnumerator FadeIcon()
        {
            var timer = 0f;
            var time = 0.1f;

            while (timer <= time)
            {
                timer += Time.deltaTime;
                var lerpProgress = timer / time;
                spriteRenderer.color = Color.Lerp(transparent, visible, lerpProgress);
                transform.localPosition = Vector3.Lerp(lerpFrom, lerpTo, lerpProgress / 2f);
                yield return null;
            }
            lerpFrom = transform.localPosition;
            timer = 0f;
            time = 0.2f;

            while (timer <= time)
            {
                timer += Time.deltaTime;
                var lerpProgress = timer / time;
                spriteRenderer.color = Color.Lerp(visible, transparent, lerpProgress);
                transform.localPosition = Vector3.Lerp(lerpFrom, lerpTo, lerpProgress);
                yield return null;
            }

            isFading = false;
        }
    }
}