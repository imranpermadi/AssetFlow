<template>
  <div ref="container" class="p-blockui-container">
    <!-- <ProgressSpinner style="z-index:1000000;position:absolute;top:40%;left:50%" v-if="!blocked"/> -->
    <div class="preloader-container" v-if="blocked">
      <div class="fulfilling-bouncing-circle-spinner">
        <div class="circle"></div>
        <div class="orbit"></div>
        <div class="pupil"></div>
      </div>
    </div>
    <slot> </slot>
  </div>
</template>

<script>
import { DomHandler, ZIndexUtils } from "primevue/utils";

export default {
  name: "BlockUI",
  emits: ["block", "unblock"],
  props: {
    blocked: {
      type: Boolean,
      default: false,
    },
    fullScreen: {
      type: Boolean,
      default: false,
    },
    baseZIndex: {
      type: Number,
      default: 0,
    },
    autoZIndex: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      zIndex: this.baseZIndex,
    };
  },
  mask: null,
  mounted() {
    if (this.blocked) {
      this.block();
    }
  },
  watch: {
    blocked(newValue) {
      if (newValue === true) this.block();
      else this.unblock();
    },
  },
  methods: {
    block() {
      let styleClass =
        "p-blockui p-component-overlay p-component-overlay-enter";
      if (this.fullScreen) {
        styleClass += " p-blockui-document";
        this.mask = document.createElement("div");
        this.mask.setAttribute("class", styleClass);
        document.body.appendChild(this.mask);
        DomHandler.addClass(document.body, "p-overflow-hidden");
        document.activeElement.blur();
      } else {
        this.mask = document.createElement("div");
        this.mask.setAttribute("class", styleClass);
        this.$refs.container.appendChild(this.mask);
      }

      if (this.autoZIndex) {
        ZIndexUtils.set(
          "modal",
          this.mask,
          this.baseZIndex + this.$primevue.config.zIndex.modal
        );
      }

      this.$emit("block");
    },
    unblock() {
      DomHandler.addClass(this.mask, "p-component-overlay-leave");
      // this.mask.addEventListener('transitionend', () => {
      //     this.removeMask();
      // });
      this.removeMask();
    },
    removeMask() {
      ZIndexUtils.clear(this.mask);
      if (this.fullScreen) {
        document.body.removeChild(this.mask);
        DomHandler.removeClass(document.body, "p-overflow-hidden");
      } else {
        this.$refs.container.removeChild(this.mask);
      }

      this.$emit("unblock");
    },
  },
};
</script>

<style>
.p-blockui-container {
  position: relative;
}

.p-blockui {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
}

.p-blockui.p-component-overlay {
  position: absolute;
}

.p-blockui-document.p-component-overlay {
  position: fixed;
}

.preloader-container {
  position: absolute;
  left: 50%;
  top: 30%;
  z-index: 9999;
}
.fulfilling-bouncing-circle-spinner,
.fulfilling-bouncing-circle-spinner * {
  box-sizing: border-box;
}

.fulfilling-bouncing-circle-spinner {
  height: 100px;
  width: 100px;
  position: relative;
  animation: fulfilling-bouncing-circle-spinner-animation infinite 4000ms ease;
}

.fulfilling-bouncing-circle-spinner .orbit {
  height: 100px;
  width: 100px;
  position: absolute;
  top: 0;
  left: 0;
  border-radius: 50%;
  border: calc(60px * 0.03) solid #d62d20;
  animation: fulfilling-bouncing-circle-spinner-orbit-animation infinite 4000ms
    ease;
}

.fulfilling-bouncing-circle-spinner .pupil {
  height: 100px;
  width: 100px;
  position: absolute;
  top: 0;
  left: 0;
  border-radius: 50%;
  border: calc(60px * 0.03) solid #d62d20;
  animation: fulfilling-bouncing-circle-spinner-pupil-animation infinite 4000ms
    ease;
}

.fulfilling-bouncing-circle-spinner .circle {
  height: 100px;
  width: 100px;
  color: #d62d20;
  display: block;
  border-radius: 50%;
  position: relative;
  border: calc(60px * 0.1) solid #d62d20;
  animation: fulfilling-bouncing-circle-spinner-circle-animation infinite 4000ms
    ease;
  transform: rotate(0deg) scale(1);
}

@keyframes fulfilling-bouncing-circle-spinner-animation {
  0% {
    transform: rotate(0deg);
  }

  100% {
    transform: rotate(360deg);
  }
}

@keyframes fulfilling-bouncing-circle-spinner-orbit-animation {
  0% {
    transform: scale(1);
  }
  16.7% {
    transform: scale(1);
    border-color: rgb(255, 62, 62);
  }
  33.4% {
    border-color: #ffa700;
  }
  50% {
    transform: scale(1);
    border-color: seashell;
  }
  62.5% {
    transform: scale(0.8);
  }
  75% {
    transform: scale(1);
    border-color: lightskyblue;
  }
  87.5% {
    transform: scale(0.8);
    border-color: lightblue;
  }
  100% {
    transform: scale(1);
    border-top-color: #d62d20;
  }
}


@keyframes fulfilling-bouncing-circle-spinner-pupil-animation {
  0% {
    transform: scale(1);
  }
  16.7% {
    transform: scale(1);
    border-color: rgb(255, 62, 62);
  }
  33.4% {
    border-color: #ffa700;
  }
  50% {
    transform: scale(1);
    border-color: seashell;
  }
  62.5% {
    transform: scale(0.3);
    border-width: calc(60px * 0.05);;
    border-color: lightskyblue;
  }
  75% {
    transform: scale(1);
    border-width: calc(60px * 0.03);
    border-color: lightskyblue;
  }
  87.5% {
    transform: scale(0.3);
    border-width: calc(60px * 0.05);
    border-color: lightblue;
  }
  100% {
    transform: scale(1);
    border-width: calc(60px * 0.03);
    border-top-color: #d62d20;
  }
}

@keyframes fulfilling-bouncing-circle-spinner-circle-animation {
  0% {
    transform: scale(1);
    border-color: transparent;
    border-top-color: inherit;
  }
  16.7% {
    border-color: transparent;
    border-top-color: rgb(255, 62, 62);
    border-right-color: rgb(255, 62, 62);
  }
  33.4% {
    border-color: transparent;
    border-top-color: #ffa700;
    border-right-color: #ffa700;
    border-bottom-color: #ffa700;
  }
  50% {
    border-color: seashell;
    transform: scale(1);
  }
  62.5% {
    border-color: lightcyan;
    transform: scale(1.4);
  }
  75% {
    border-color: lightskyblue;
    transform: scale(1);
    opacity: 1;
  }
  87.5% {
    border-color: lightblue;
    transform: scale(1.4);
  }
  100% {
    border-color: transparent;
    border-top-color: #d62d20;
    transform: scale(1);
  }
}
</style>
