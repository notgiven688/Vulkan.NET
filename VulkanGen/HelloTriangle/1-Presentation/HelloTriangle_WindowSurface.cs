using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Evergine.Bindings.Vulkan;

namespace KHRRTXHelloTriangle
{
    public unsafe partial class HelloTriangle
    {
        private VkSurfaceKHR surface;

        private void CreateSurface()
        {
            var result = GLFW.glfwCreateWindowSurface(this.instance, window, (VkAllocationCallbacks*)0, out surface);

            if(result != VkResult.VK_SUCCESS)
            {
                throw new Exception("Could not create window surface");
            }        
        }
    }
}
