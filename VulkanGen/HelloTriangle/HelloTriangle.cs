using System;
using Evergine.Bindings.Vulkan;

namespace KHRRTXHelloTriangle
{
    public unsafe partial class HelloTriangle
    {
        const uint WIDTH = 800;
        const uint HEIGHT = 600;       

        private IntPtr window;

        private void InitWindow()
        {
            GLFW.glfwInit();

            GLFW.glfwWindowHint(GLFW.GLFW_CLIENT_API, GLFW.GLFW_NO_API);
            GLFW.glfwWindowHint(GLFW.GLFW_RESIZABLE, GLFW.GLFW_FALSE);

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes("Vulkan");
            window = GLFW.glfwCreateWindow((int)WIDTH, (int)HEIGHT, bytes, IntPtr.Zero, IntPtr.Zero);

            if (window == IntPtr.Zero)
            {
                GLFW.glfwTerminate();
                throw new Exception("Unable to create glfw window");
            }
        }

        private void InitVulkan()
        {
            this.CreateInstance();

            this.SetupDebugMessenger();

            this.CreateSurface();

            this.PickPhysicalDevice();

            this.CreateLogicalDevice();

            this.CreateSwapChain();

            this.CreateImageViews();

            this.CreateRenderPass();

            this.CreateGraphicsPipeline();

            this.CreateFramebuffers();

            this.CreateCommandPool();

            this.CreateCommandBuffers();

            this.CreateSemaphores();
        }

        private void MainLoop()
        {           
            while(GLFW.glfwWindowShouldClose(window) == GLFW.GLFW_FALSE)
            {
                this.DrawFrame();
                GLFW.glfwPollEvents();
            }

            Helpers.CheckErrors(VulkanNative.vkDeviceWaitIdle(this.device));
        }

        private void CleanUp()
        {
            VulkanNative.vkDestroySemaphore(this.device, this.renderFinishedSemaphore, null);
            VulkanNative.vkDestroySemaphore(this.device, this.imageAvailableSemaphore, null);

            VulkanNative.vkDestroyCommandPool(this.device, this.commandPool, null);

            foreach (var framebuffer in this.swapChainFramebuffers)
            {
                VulkanNative.vkDestroyFramebuffer(this.device, framebuffer, null);
            }

            VulkanNative.vkDestroyPipeline(this.device, this.graphicsPipeline, null);

            VulkanNative.vkDestroyPipelineLayout(this.device, this.pipelineLayout, null);

            VulkanNative.vkDestroyRenderPass(this.device, this.renderPass, null);

            foreach (var imageView in this.swapChainImageViews)
            {
                VulkanNative.vkDestroyImageView(this.device, imageView, null);
            }

            VulkanNative.vkDestroySwapchainKHR(this.device, this.swapChain, null);

            VulkanNative.vkDestroyDevice(this.device, null);

            this.DestroyDebugMessenger();

            VulkanNative.vkDestroySurfaceKHR(this.instance, this.surface, null);

            VulkanNative.vkDestroyInstance(this.instance, null);

            GLFW.glfwDestroyWindow(window);
            GLFW.glfwTerminate();
        }

        public void Run()
        {
            this.InitWindow();

            this.InitVulkan();

            this.MainLoop();

            this.CleanUp();
        }   
    }
}
